import { SyntheticEvent, useEffect, useState } from "react";
import "../CSS/About.css";
import axios from "axios";
import { ReadAbout, PostAbout, IsElevatedUser } from "../endpoints";
import { useNavigate } from "react-router-dom";

function About() {
  const [aboutText, setAboutText] = useState(Object);

  const navigate = useNavigate();

  useEffect(() => {
    getAbout();
    getIsElevatedUser();
  }, []);

  async function getAbout() {
    await axios
      .get(ReadAbout, {
        headers: { "Content-Type": "application/json" },
      })
      .then((response) => {
        setAboutText(response.data);
      });
  }

  async function getIsElevatedUser() {
    await axios
      .get(IsElevatedUser, {
        withCredentials: true,
        headers: { "Content-Type": "application/json" },
      })
      .then((response) => {
        let isElevatedUser = response.data;
        if (isElevatedUser === true) {
          btnSubmit_setHidden(false);
          btnSubmit_setDisabled(true);
          txtAbout_setReadOnly(false);
        }
      });
  }

  function btnSubmit_setHidden(hidden: boolean) {
    let btnSubmit = document.getElementById("btnSubmit") as HTMLButtonElement;
    btnSubmit.hidden = hidden;
  }

  function btnSubmit_setDisabled(disabled: boolean) {
    let btnSubmit = document.getElementById("btnSubmit") as HTMLButtonElement;
    btnSubmit.disabled = disabled;
  }

  function txtAbout_setReadOnly(readOnly: boolean) {
    let txtAbout = document.getElementById("txtAbout") as HTMLTextAreaElement;
    txtAbout.readOnly = readOnly;
  }

  const handleSubmit = (e: SyntheticEvent) => {
    e.preventDefault();

    axios
      .post(
        PostAbout,
        JSON.stringify({
          text: aboutText,
        }),
        {
          headers: { "Content-Type": "application/json" },
        }
      )
      .then(() => {
        btnSubmit_setDisabled(true);
      })
      .catch((error) => {
        if (error) {
          getAbout();
        }
      });
  };

  let handleBack = () => {
    return navigate("/");
  };

  return (
    <div className="whole-about-container text-area-container">
      <button id="btnBack" className="btn btn-primary" onClick={handleBack}>
        Home
      </button>
      <textarea
        id="txtAbout"
        className="textarea"
        rows={Number(25)}
        readOnly
        value={aboutText.text}
        onChange={(e) => {
          setAboutText(e.target.value);
          btnSubmit_setDisabled(false);
        }}
      />
      <button id="btnSubmit" className="btn btn-primary" hidden type="submit" onClick={handleSubmit}>
        Submit
      </button>
    </div>
  );
}

export default About;
