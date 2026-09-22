import { Router } from 'express';
import { executeQuery } from '../db.js';

const router = Router();
const COOLDOWN_MS = 5 * 60 * 1000; // 5 minutes cooldown

// POST route to claim hourly rewards
router.post('/claim/:id', async (req, res) => {
  const rewardId = parseInt(req.params.id, 10);

  try {
    const rows = await executeQuery(
      'SELECT * FROM hourly_rewards WHERE id = ?',
      [rewardId]
    );

    if (!rows.length) {
      return res.status(404).json({ error: 'Reward not found' });
    }

    const reward = rows[0];
    const now = new Date();
    const lastClaimed = reward.last_claimed_at
      ? new Date(reward.last_claimed_at)
      : null;

    if (lastClaimed && lastClaimed > new Date(now.getTime() - COOLDOWN_MS)) {
      const timeRemaining = (lastClaimed.getTime() + COOLDOWN_MS) - now.getTime();
      return res.status(429).json({
        error: `Reward is on cooldown. Try again in ${Math.ceil(timeRemaining / 60000)} minute(s).`,
      });
    }

    await executeQuery(
      'UPDATE hourly_rewards SET last_claimed_at = NOW() WHERE id = ?',
      [rewardId]
    );

    res.json({
      success: true,
      reward: { name: reward.reward_name, quantity: reward.reward_quantity, type: reward.reward_type },
    });
  } catch (err) {
    console.error(err);
    res.status(500).json({ error: 'Internal server error' });
  }
});

// GET route to check the status of a reward
router.get('/status/:id', async (req, res) => {
  const rewardId = parseInt(req.params.id, 10);

  try {
    const rows = await executeQuery(
      'SELECT reward_quantity, last_claimed_at FROM hourly_rewards WHERE id = ?',
      [rewardId]
    );

    if (!rows.length) {
      return res.status(404).json({ error: 'Reward not found' });
    }

    const reward = rows[0];
    const lastClaimedAt = reward.last_claimed_at
      ? new Date(reward.last_claimed_at).toISOString()
      : null;

    let timeLeftSeconds = 0;
    if (lastClaimedAt) {
      const lastClaimedDate = new Date(lastClaimedAt);
      const hourFromLast = lastClaimedDate.getTime() + COOLDOWN_MS;
      const now = Date.now();
      if (now < hourFromLast) {
        timeLeftSeconds = Math.ceil((hourFromLast - now) / 1000);
      }
    }

    res.json({
      success: true,
      lastClaimedAt,
      rewardQuantity: reward.reward_quantity,
      timeLeftSeconds,
    });
  } catch (err) {
    console.error(err);
    res.status(500).json({ error: 'Internal server error' });
  }
});

export default router;