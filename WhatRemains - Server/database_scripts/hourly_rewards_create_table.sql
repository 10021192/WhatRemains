CREATE TABLE hourly_rewards (
    id INT AUTO_INCREMENT PRIMARY KEY,
    reward_name VARCHAR(255) NOT NULL,
    reward_quantity INT NOT NULL,
    reward_type ENUM('coins', 'items', 'discount') NOT NULL,
    last_claimed_at DATETIME DEFAULT NULL
    -- Optionally you can add next_claim_at DATETIME or claim_interval INT
);