import React from "react";
import { useSearchParams } from "react-router-dom";
import BooksComp from "../components/BooksComp";
import Footer from "../components/Footer";
import Header from "../components/Header";

function Books() {
  const [searchParams] = useSearchParams();

  const keyParams = searchParams.get("keywords");
  const genreParams = searchParams.get("genres");

  return (
    <div className="flex flex-col win-h-screen justify-between">
      <Header />
      <BooksComp keywords={keyParams ?? ""} genre={genreParams ?? ""} />
      <Footer />
    </div>
  );
}

export default Books;
