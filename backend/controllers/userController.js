const asyncHandler = require("express-async-handler");
const bcrypt = require("bcrypt");
const jwt = require("jsonwebtoken");
const User = require("../models/userModel");

//@route POST /api/users/register
//@access public
//@desc register new user
const registerUser = asyncHandler(async (req, res) => {
    const { username, email, password, arCombatKey } = req.body;

    //this may not be necessary with client validation
    if (!username || !email || !password || !arCombatKey) {
      res.status(400);
      throw new Error("All fields are mandatory!");
    }

    //check if user already exists
    const userAvailable = await User.findOne({ email });
    if (userAvailable) {
      res.status(400);
      throw new Error("User already registered!");
    }

    //check if arCombatKey already exists
    const arCombatAvailable = await User.findOne({ arCombatKey });
    if (arCombatAvailable) {
      res.status(400);
      throw new Error("AR Combat Key already registered!");
    }
  
    //Hash password
    const hashedPassword = await bcrypt.hash(password, 10);
    console.log("Hashed Password: ", hashedPassword);
    const user = await User.create({
      username,
      email,
      password: hashedPassword,
    });
  
    console.log(`User created ${user}`);

    //send response back after succcessful registation
    if (user) {
      res.status(201).json({ _id: user.id, email: user.email });
    } else {
      res.status(400);
      throw new Error("User data is not valid");
    }

    res.json({ message: "Register the user" });


  });
  