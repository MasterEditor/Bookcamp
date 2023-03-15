import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import AdminHeader from "../components/AdminHeader";
import Footer from "../components/Footer";
import FromAuthor from "../components/FromAuthor";
import Header from "../components/Header";
import OneBookFeedback from "../components/OneBookFeedback";
import OneBookInfo from "../components/OneBookInfo";
import { ADMIN } from "../constants/roles";
import { useAppSelector } from "../hooks/useAppSelector";
import { IBook } from "../models/IBook";
import { booksApi } from "../services/booksApi";

function OneBookNew() {
  const { id } = useParams();
  const { role } = useAppSelector((state) => state.user.user);
  const [book, setBook] = useState<IBook>();
  const [showAlert, setShowAlert] = useState(false);
  const { data: bookData } = booksApi.useGetBookQuery(id!);

  useEffect(() => {
    setBook(bookData?.body!);
  }, [bookData]);

  const handleShowAlert = () => {
    if (showAlert) {
      window.scrollTo({
        top: 0,
        behavior: "smooth",
      });
    } else {
      setShowAlert(true);
    }
  };

  return (
    <>
      {role === ADMIN ? <AdminHeader /> : <Header />}
      <OneBookInfo
        id={id!}
        book={book!}
        handleShowAlert={handleShowAlert}
        showAlert={showAlert}
      />
      <FromAuthor id={id!} author={book?.author!} />
      <OneBookFeedback id={id!} handleShowAlert={handleShowAlert} />
      <Footer />
    </>
  );
}

export default OneBookNew;
