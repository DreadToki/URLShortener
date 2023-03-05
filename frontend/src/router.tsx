import { createBrowserRouter } from "react-router-dom";
import ShortUrlCard, { ShortUrlCardLoader } from "./Pages/ShortUrlCard";
import About from "./Pages/About";
import Main from "./Pages/Main";
import Signing from "./Pages/Signing";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <Main />,
  },
  {
    path: "card/:shortUrl",
    element: <ShortUrlCard />,
    loader: ShortUrlCardLoader,
  },
  {
    path: "login",
    element: <Signing path={"login"} />,
  },
  {
    path: "register",
    element: <Signing path={"register"} />,
  },
  {
    path: "about",
    element: <About />,
  },
]);
