import mysql from 'mysql2/promise';
import dotenv from 'dotenv';

dotenv.config();

// Create a connection pool with proper limits
const pool = mysql.createPool({
  host: process.env.DB_HOST,
  user: process.env.DB_USER,
  password: process.env.DB_PASS,
  database: process.env.DB_NAME,
  port: process.env.DB_PORT,
  waitForConnections: true,
  connectionLimit: 5, // Adjusted to match plan limit
  queueLimit: 0, // Unlimited queue for requests
});

// Wrapper function to execute queries safely
export const executeQuery = async (query, params = []) => {
  let connection;
  try {
    connection = await pool.getConnection(); // Get connection from the pool
    const [results] = await connection.query(query, params);
    return results;
  } catch (error) {
    console.error('Database query error:', error.message);
    throw error;
  } finally {
    if (connection) connection.release(); // Always release the connection
  }
};

export default pool;