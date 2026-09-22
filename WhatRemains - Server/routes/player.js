import { Router } from 'express';
import { executeQuery } from '../db.js';

const router = Router();

// POST route to update player's discount
router.post('/:id/discount', async (req, res) => {
  try {
    const playerId = req.params.id;
    let { score } = req.body;
    if (!score) score = 0;

    let discount = Math.min(score, 50); // Cap discount at 50

    await executeQuery(
      `UPDATE players 
       SET discount = ?, 
           discount_expires_at = DATE_ADD(NOW(), INTERVAL 10 MINUTE)
       WHERE id = ?`,
      [discount, playerId]
    );

    return res.json({ success: true, discount });
  } catch (err) {
    console.error(err);
    return res.status(500).json({ error: 'Internal server error' });
  }
});

// GET route to fetch player details
router.get('/:id', async (req, res) => {
  try {
    const playerId = req.params.id;
    const rows = await executeQuery(
      `SELECT coins, discount, discount_expires_at 
       FROM players 
       WHERE id = ?`,
      [playerId]
    );

    if (!rows.length) {
      return res.status(404).json({ error: 'Player not found' });
    }

    let { coins, discount, discount_expires_at } = rows[0];
    const now = new Date();
    if (discount_expires_at && new Date(discount_expires_at) < now) {
      discount = 0;
      await executeQuery(
        `UPDATE players 
         SET discount = 0, discount_expires_at = NULL 
         WHERE id = ?`,
        [playerId]
      );
    }

    res.json({ success: true, player: { id: playerId, coins, discount } });
  } catch (err) {
    console.error(err);
    res.status(500).json({ error: 'Internal server error' });
  }
});

export default router;