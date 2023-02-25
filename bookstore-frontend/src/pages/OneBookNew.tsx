import React from "react";
import AdminHeader from "../components/AdminHeader";
import Header from "../components/Header";
import OneBookInfo from "../components/OneBookInfo";
import { ADMIN } from "../constants/roles";
import { useAppSelector } from "../hooks/useAppSelector";

function OneBookNew() {
  const { favourites, imageUrl, name, role } = useAppSelector(
    (state) => state.user.user
  );

  return (
    <>
      {role === ADMIN ? <AdminHeader /> : <Header />}
      <OneBookInfo />
    </>
  );
}

export default OneBookNew;
