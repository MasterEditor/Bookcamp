import React from "react";
import { useCookies } from "react-cookie";
import { Navigate, Outlet } from "react-router-dom";
import { useAppSelector } from "../hooks/useAppSelector";

interface ProtectedRouteProps {
  allowedRoles: string[];
}

function ProtectedRout({ allowedRoles }: ProtectedRouteProps) {
  const name = useAppSelector((state) => state.user.user.name);
  const img = localStorage.getItem("user_image");
  const [cookies] = useCookies(["bc_role"]);

  return allowedRoles.find((allowed) => allowed === cookies.bc_role) ? (
    <Outlet />
  ) : name && img ? (
    <Navigate to="/unauthorized" />
  ) : (
    <Navigate to="/login" />
  );
}

export default ProtectedRout;
