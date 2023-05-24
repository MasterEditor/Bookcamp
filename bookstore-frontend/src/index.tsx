import React from "react";
import ReactDOM from "react-dom/client";
import { Provider } from "react-redux";
import { BrowserRouter } from "react-router-dom";
import App from "./App";
import { setUpStore } from "./store/store";
import "./index.css";
import "./i18n";

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);

const store = setUpStore();

root.render(
  <React.StrictMode>
    <Provider store={store}>
      <BrowserRouter>
        <App />
      </BrowserRouter>
    </Provider>
  </React.StrictMode>
);
