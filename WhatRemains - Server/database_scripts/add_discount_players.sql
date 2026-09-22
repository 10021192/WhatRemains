ALTER TABLE players ADD discount INT DEFAULT 0;
UPDATE players SET discount=0 WHERE id=1;