import React from "react";
import { useCookies } from "react-cookie";
import { Navigate } from "react-router-dom";
import Footer from "../components/Footer";
import Header from "../components/Header";
import Main from "../components/Main";

function MainPage() {
  const [cookies] = useCookies(["bc_role"]);

  return cookies.bc_role ? (
    <Navigate to="/books" />
  ) : (
    <div className="flex flex-col h-full justify-between">
      <Header />
      <Main />
      <Footer />
    </div>
  );
}

export default MainPage;
