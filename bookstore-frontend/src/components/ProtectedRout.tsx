import React from "react";
import { useCookies } from "react-cookie";
import { Navigate, Outlet } from "react-router-dom";

interface ProtectedRouteProps {
  allowedRoles: string[];
}

function ProtectedRout({ allowedRoles }: ProtectedRouteProps) {
  const name = localStorage.getItem("user_name");
  const [cookies] = useCookies(["bc_role"]);
  console.log(name);

  return allowedRoles.find((allowed) => allowed === cookies.bc_role) ? (
    <Outlet />
  ) : name ? (
    <Navigate to="/unauthorized" />
  ) : (
    <Navigate to="/login" />
  );
}

export default ProtectedRout;
