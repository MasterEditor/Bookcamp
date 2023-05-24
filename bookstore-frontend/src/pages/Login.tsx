import { FetchBaseQueryError } from "@reduxjs/toolkit/dist/query";
import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import FormInput from "../components/FormInput";
import { USER } from "../constants/roles";
import { useActions } from "../hooks/useActions";
import { useInput } from "../hooks/useInput";
import { REQUIRED } from "../hooks/useValidation";
import { ILogin } from "../models/ILogin";
import { authApi } from "../services/authApi";
import { useTranslation } from "react-i18next";

function Login() {
  const { t } = useTranslation();
  const [loginUser, { isLoading, data, isSuccess, error, isError }] =
    authApi.useLoginUserMutation();

  const { setRole } = useActions();

  const navigate = useNavigate();

  const email = useInput("", REQUIRED);
  const password = useInput("", REQUIRED);
  const [errorMessage, setErrorMessage] = useState("");

  useEffect(() => {
    if (isSuccess) {
      const role = data!.body;

      setRole(data!.body);

      if (role === USER) {
        navigate("/books");
      } else {
        navigate("/admin-books");
      }
    }

    if (isError) {
      if ((error as FetchBaseQueryError).status === 400) {
        setErrorMessage("Please check your email and password");
      }
    }
  }, [isLoading]);

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    const user: ILogin = {
      email: email.bind.value,
      password: password.bind.value,
    };

    loginUser(user);
  };

  return (
    <div className="mx-auto w-[400px]">
      <div className="flex justify-center mt-20">
        <form
          onSubmit={handleSubmit}
          className="bg-white shadow-md w-full rounded-lg px-8 pt-6 pb-8 mb-4"
        >
          <h2 className="text-xl mb-8 mt-6 text-center font-bold">
            {t("loginLine1")!}
          </h2>
          <FormInput
            id="email"
            type="text"
            placeholder={t("loginLine2")!}
            input={email}
          />
          <FormInput
            id="password"
            type="password"
            placeholder={t("loginLine3")!}
            input={password}
          />

          {errorMessage && (
            <p className="text-red-500 mt-1 text-sm italic">{errorMessage}</p>
          )}
          <button
            disabled={
              !email.valid.isRegexPass ||
              !password.valid.isRegexPass ||
              isLoading
            }
            type="submit"
            className="rounded-xl bg-[#2c968f] text-white p-2 w-full mt-3 mb-4 transition duration-300 ease-in-out hover:opacity-90"
          >
            <p>{t("headerLine2")!}</p>
          </button>
          <p className="text-center">{t("loginLine4")!}</p>
          <p className="underline underline-offset-2 text-center">
            <Link to="/signup" className="text-[#cc9f36]">
              {t("mainLine2")!}
            </Link>
          </p>
        </form>
      </div>
    </div>
  );
}

export default Login;
