import React from "react";
import { useSearchParams } from "react-router-dom";
import AdminHeader from "../../components/AdminHeader";
import BooksComp from "../../components/BooksComp";

function AdminBooks() {
  const [searchParams] = useSearchParams();

  const keyParams = searchParams.get("keywords");
  const genreParams = searchParams.get("genres");

  return (
    <div className="flex flex-col h-full justify-between">
      <AdminHeader />
      <BooksComp keywords={keyParams ?? ""} genre={genreParams ?? ""} />
    </div>
  );
}

export default AdminBooks;
