import React, { SyntheticEvent, useEffect, useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import { BaseUrl, ReadUrls, ReadUser, LogoutUser, CreateUrl, IsElevatedUser } from "../endpoints";
import { UrlList } from "./ShortUrlCard";
import axios from "axios";
import { ImCross } from "react-icons/im";
import "../CSS/ModalWindow.css";
import "../CSS/ShortUrlTable.css";

export type User = {
  login: string;
};

function Main() {
  const navigate = useNavigate();
  const [urls, setUrls] = useState<UrlList[] | null>();
  const [modal, setModal] = useState(false);
  const [longUrl, setLongUrl] = useState("");
  const [createUrlError, setCreateUrlError] = useState("");
  const [user, setUser] = useState<User>();
  const [showInfoButton, setShowInfoButton] = useState(false);

  useEffect(() => {
    getUrls();
    getUser();
    getIsElevatedUser();
  }, []);

  async function getUrls() {
    await axios
      .get(ReadUrls, {
        headers: { "Content-Type": "application/json" },
      })
      .then((response) => {
        setUrls(response.data);
      });
  }

  async function getUser() {
    await axios
      .get(ReadUser, {
        withCredentials: true,
        headers: { "Content-Type": "application/json" },
      })
      .then((response) => {
        setUser(response.data);
      })
      .catch((e) => {
        setUser(e.data);
      });
  }

  async function getIsElevatedUser() {
    await axios
      .get(IsElevatedUser, {
        withCredentials: true,
      })
      .then((response) => {
        setShowInfoButton(response.data);
      })
      .catch((e) => {
        setShowInfoButton(false);
      });
  }

  const handleSubmit = async (e: SyntheticEvent) => {
    e.preventDefault();

    await axios
      .post(
        CreateUrl,
        JSON.stringify({
          longUrl: longUrl,
        }),
        {
          headers: { "Content-Type": "application/json" },
          withCredentials: true,
        }
      )
      .then((response) => getUrls())
      .catch((error) => {
        if (error.response) {
          return setCreateUrlError(error.response.data);
        }
      });
  };

  const OnLongUrlChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setLongUrl(e.target.value);
    setCreateUrlError("");
  };

  const toggleModal = () => {
    setModal(!modal);
  };

  if (modal) {
    document.body.classList.add("active-modal");
  } else {
    document.body.classList.remove("active-modal");
  }

  const handleLogout = async () => {
    await fetch(LogoutUser, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    }).then(() => {
      handleHome();
    });
  };

  function handleAbout() {
    return navigate("/about");
  }

  function handleHome() {
    getUrls();
    getUser();
    getIsElevatedUser();
  }

  function handleRegister() {
    return navigate("/register");
  }

  function handleLogin() {
    return navigate("/login");
  }

  let menu;

  if (user === null || user === undefined) {
    menu = (
      <>
        <button className="btn btn-primary" onClick={handleLogin}>
          Login
        </button>
        <button className="btn btn-primary" onClick={handleRegister}>
          Register
        </button>
      </>
    );
  } else {
    menu = (
      <>
        <button className="btn btn-primary" onClick={handleLogout}>
          Logout
        </button>
        <button id="btn-modal" className="btn btn-primary" onClick={toggleModal}>
          Add Short URL
        </button>
      </>
    );
  }

  return (
    <>
      <div className="btns-menu">
        <button className="btn btn-primary" onClick={handleHome}>
          Home
        </button>
        <button className="btn btn-primary" onClick={handleAbout}>
          About
        </button>
        {menu}
      </div>
      <h1>{user?.login ? "Welcome, " + user.login + "!" : "You are not logged in."}</h1>
      {modal && (
        <div>
          <div className="overlay" onClick={toggleModal}></div>
          <div className="modal-content">
            <h1>Extremely Tiny URL</h1>
            <form onSubmit={handleSubmit}>
              <label>Enter new url:</label>
              <input type="text" className="form-control" placeholder="https://www.example.com/" onChange={(e) => OnLongUrlChange(e)} />
              <label id="lbl-error">{createUrlError}</label>
              <input className="btn-submit" type="submit" value="Make it shorter" />
              <ImCross size={20} className="btn-cross" color="#b1203d" onClick={toggleModal} />
            </form>
          </div>
        </div>
      )}
      <div className="whole-table-container">
        <table className="table">
          <thead>
            <tr>
              <th scope="col">Short Url</th>
              <th scope="col">Long Url</th>
              <th scope="col">Action</th>
            </tr>
          </thead>
          <tbody className="table-body">
            {urls
              ? urls.map((urlList) => {
                  return (
                    <tr key={urlList.shortUrl}>
                      <td>
                        <NavLink to={BaseUrl + urlList.shortUrl}>{BaseUrl + urlList.shortUrl}</NavLink>
                      </td>
                      <td>
                        <NavLink to={urlList.longUrl}>{urlList.longUrl}</NavLink>
                      </td>
                      <td>
                        {user?.login === urlList.createdBy || showInfoButton ? (
                          <NavLink id="btn-info" to={`card/${urlList.shortUrl}`}>
                            Info
                          </NavLink>
                        ) : (
                          ""
                        )}
                      </td>
                    </tr>
                  );
                })
              : null}
          </tbody>
        </table>
      </div>
    </>
  );
}

export default Main;
