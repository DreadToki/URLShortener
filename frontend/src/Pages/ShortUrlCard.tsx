import axios from "axios";
import { useLoaderData, LoaderFunctionArgs, useNavigate } from "react-router-dom";
import { DeleteUrl, ReadUrl } from "../endpoints";
import "../CSS/ShortUrlCard.css";

export type UrlList = {
  shortUrl: string;
  longUrl: string;
  createdBy: string;
  createdDateTime: string;
};

export const ShortUrlCardLoader = async ({ params }: LoaderFunctionArgs): Promise<UrlList> => {
  let results = await fetch(`${ReadUrl + "/" + params.shortUrl}`);
  let urlCard = await results.json();
  return urlCard;
};

const ShortUrlCard = () => {
  let props = useLoaderData() as UrlList;
  let navigate = useNavigate();

  function deleteUri() {
    axios.delete(`${DeleteUrl + "/" + props.shortUrl}`).then(() => {
      return navigate("/");
    });
  }

  const handleBack = () => {
    return navigate("/");
  };

  return (
    <div className="whole-url-container">
      <button id="btnBack" className="btn btn-primary" onClick={handleBack}>
        Home
      </button>
      <div className="form-url-group">
        <label className="shortUri">Short Url: {props.shortUrl}</label>
        <label>Long Url: {props.longUrl}</label>
        <label>Created By: {props.createdBy}</label>
        <label>Created Date: {props.createdDateTime}</label>
        <button className="btn btn-primary" type="submit" onClick={deleteUri}>
          Delete
        </button>
      </div>
    </div>
  );
};

export default ShortUrlCard;
