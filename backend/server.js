const express = require("express");
const connectDb = require("./config/db");
const errorHandler = require("./middleware/errorHandler");
const dotenv = require("dotenv").config();

connectDb();
const app = express();

const port = process.env.PORT || 5000;
app.use("/api/users", require("./routes/userRoutes"));
app.use(errorHandler);
app.use(express.json());

app.listen(port, () => {
    console.log(`Server running on port ${port}`);
  });
