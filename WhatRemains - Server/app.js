// app.js
import express from 'express'; // Import the Express library
import dotenv from 'dotenv'; // Import the dotenv library to load environment variables
import hourlyRewardsRouter from './routes/hourlyRewards.js'; // Import the hourly rewards router
import playerRouter from './routes/player.js'; // Import the player router

dotenv.config(); // Load environment variables from the .env file
const app = express(); // Create an Express application

app.use(express.json()); // Middleware to parse JSON bodies

// Mount the hourly rewards router at the /hourly_rewards path
app.use('/hourly_rewards', hourlyRewardsRouter);
// Mount the player router at the /player path
app.use('/player', playerRouter);

app.get('/', (req, res) => {
  res.json({ message: 'Welcome to the API!' });
});

// Simple test route to check if the server is running
app.get('/test-connection', (req, res) => {
  res.json({ success: true, message: 'Server is up!' });
});

// Start the server on the port specified in the environment variables or default to 3000
const PORT = process.env.PORT || 3000;
app.listen(PORT, '0.0.0.0', () => {
  console.log(`Server listening on port ${PORT}`);
});