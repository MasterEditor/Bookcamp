import React from "react";
import { Navigate } from "react-router-dom";
import Footer from "../components/Footer";
import Header from "../components/Header";
import Main from "../components/Main";
import { useAppSelector } from "../hooks/useAppSelector";

function MainPage() {
  const user = useAppSelector((x) => x.user.user);

  return user.role ? (
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
