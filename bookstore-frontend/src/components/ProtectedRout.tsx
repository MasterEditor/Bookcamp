import React, { useEffect, useState } from "react";
import { Navigate, Outlet } from "react-router-dom";
import { UNKNOWN } from "../constants/roles";
import { useActions } from "../hooks/useActions";
import { useAppSelector } from "../hooks/useAppSelector";
import { authApi } from "../services/authApi";

interface ProtectedRouteProps {
  allowedRoles: string[];
}

function ProtectedRout({ allowedRoles }: ProtectedRouteProps) {
  const { data, isLoading, isSuccess, isError } = authApi.useCheckUserQuery();
  const [role, setLocalRole] = useState("");
  const { setRole } = useActions();

  useEffect(() => {
    if (isSuccess) {
      setLocalRole(data);
      setRole(data);
    }

    if (isError) {
      setLocalRole(UNKNOWN);
    }
  }, [isLoading, isSuccess, isError]);

  const user = useAppSelector((x) => x.user.user);

  return role ? (
    allowedRoles.find((allowed) => allowed === role) ? (
      <Outlet />
    ) : user.name ? (
      <Navigate to="/unauthorized" />
    ) : (
      <Navigate to="/login" />
    )
  ) : null;
}

export default ProtectedRout;
