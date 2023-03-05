import axios from "axios";
import React, { SyntheticEvent, useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import { LoginUser, RegisterUser } from "../endpoints";
import * as CryptoJS from "crypto-js";
import "../CSS/Signing.css";

function Signing(props: { path: string }) {
  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");
  const [errorPost, setErrorPost] = useState("");

  const navigate = useNavigate();

  const OnLoginChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setLogin(e.target.value);
    setErrorPost("");
  };

  const OnPasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setPassword(e.target.value);
    setErrorPost("");
  };

  const handleBack = () => {
    return navigate("/");
  };

  const submitLogin = async (e: SyntheticEvent) => {
    e.preventDefault();

    let hashedPassword = CryptoJS.SHA256(password).toString();

    await axios
      .post(
        LoginUser,
        JSON.stringify({
          login: login,
          password: hashedPassword,
        }),
        {
          headers: {
            "Content-Type": "application/json",
          },
          withCredentials: true,
        }
      )
      .then(() => {
        return navigate("/");
      })
      .catch((error) => {
        if (error.response) {
          return setErrorPost(error.response.data);
        }
      });
  };

  const submitRegister = async (e: SyntheticEvent) => {
    e.preventDefault();

    let hashedPassword = CryptoJS.SHA256(password).toString();

    await axios
      .post(
        RegisterUser,
        JSON.stringify({
          login: login,
          password: hashedPassword,
        }),
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      )
      .then(() => {
        return navigate("/login");
      })
      .catch((error) => {
        if (error.response) {
          return setErrorPost(error.response.data);
        }
      });
  };

  let page;

  if (props.path === "login") {
    page = (
      <>
        <label>Don't have an account? </label>
        <NavLink className="btn-redirect" to="/register" onClick={() => setErrorPost("")}>
          Sign up for free!
        </NavLink>
      </>
    );
  } else if (props.path === "register") {
    page = (
      <>
        <label>Already have an account? </label>
        <NavLink className="btn-redirect" to="/login" onClick={() => setErrorPost("")}>
          Sign in!
        </NavLink>
      </>
    );
  }

  return (
    <div className="whole-signing-container">
      <button id="btnBack" className="btn btn-primary" onClick={handleBack}>
        Home
      </button>
      <form onSubmit={props.path === "register" ? submitRegister : submitLogin}>
        <div className="form-signing-group">
          <h1>{props.path === "register" ? "Sign up" : "Sign in"}</h1>
          <label>Enter Login</label>
          <input type="text" className="form-control" placeholder="Enter login" required onChange={(e) => OnLoginChange(e)} />
          <label>Password</label>
          <input type="password" className="form-control" placeholder="Password" required onChange={(e) => OnPasswordChange(e)} />
          <label id="lbl-error">{errorPost}</label>
          {page}
          <button type="submit" className="btn btn-primary">
            {props.path === "register" ? "Register" : "Login"}
          </button>
        </div>
      </form>
    </div>
  );
}

export default Signing;
