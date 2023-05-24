import { FetchBaseQueryError } from "@reduxjs/toolkit/dist/query";
import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import FormInput from "../components/FormInput";
import { maxDate, minDate } from "../constants/regularExpression";
import { useInput } from "../hooks/useInput";
import {
  BIRTHDAY,
  EMAIL_REGEX,
  NAME_REGEX,
  PASSWORD_REGEX,
} from "../hooks/useValidation";
import { ISignup } from "../models/ISignup";
import { authApi } from "../services/authApi";
import { useTranslation } from "react-i18next";

function SignUp() {
  const { t } = useTranslation();
  const [signUpUser, { isLoading, isSuccess, error, isError }] =
    authApi.useSignupUserMutation();

  const [errorMessage, setErrorMessage] = useState("");
  const [gender, setGender] = useState(1);

  const navigate = useNavigate();

  const name = useInput("", NAME_REGEX);
  const email = useInput("", EMAIL_REGEX);
  const password = useInput("", PASSWORD_REGEX);
  const birthday = useInput("", BIRTHDAY);

  useEffect(() => {
    if (isSuccess) {
      navigate("/login");
    }

    if (isError) {
      if ((error as FetchBaseQueryError).status === 400) {
        setErrorMessage("User with such email already exists. Login please.");
      }
    }
  }, [isLoading]);

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    const user: ISignup = {
      name: name.bind.value,
      email: email.bind.value,
      password: password.bind.value,
      birthday: birthday.bind.value,
      gender: gender,
    };

    signUpUser(user);
  };

  return (
    <div className="mx-auto w-[400px] mb-16">
      <div className="flex justify-center mt-20">
        <form
          onSubmit={handleSubmit}
          className="bg-white shadow-md w-full rounded-lg px-8 pt-6 pb-8 mb-4"
        >
          <h2 className="text-xl mb-8 mt-6 text-center font-bold">
            {t("signupLine1")!}
          </h2>
          <FormInput
            id="username"
            type="text"
            placeholder={t("signupLine2")!}
            input={name}
          />
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
          <FormInput
            id="date"
            type="date"
            placeholder={t("signupLine3")!}
            input={birthday}
            max={maxDate}
            min={minDate}
          />
          <input
            id="man"
            type="radio"
            name="status"
            defaultChecked
            onClick={() => setGender(1)}
          />
          <label htmlFor="man" className="mr-2">
            {t("signupLine4")!}
          </label>
          <input
            id="woman"
            type="radio"
            name="status"
            onClick={() => setGender(2)}
          />
          <label htmlFor="woman">{t("signupLine5")!}</label>
          {errorMessage && (
            <p className="text-red-500 mt-1 text-sm italic">{errorMessage}</p>
          )}

          <button
            disabled={
              !name.valid.isRegexPass ||
              !email.valid.isRegexPass ||
              !password.valid.isRegexPass ||
              isLoading
            }
            type="submit"
            className="rounded-xl bg-[#2c968f] text-white p-2 w-full mt-3 mb-4 transition duration-300 ease-in-out hover:opacity-90"
          >
            {t("signupLine6")!}
          </button>
          <p className="text-center">{t("signupLine7")!}</p>
          <p className="underline underline-offset-2 text-center">
            <Link to="/login" className="text-[#cc9f36]">
              {t("headerLine2")!}
            </Link>
          </p>
        </form>
      </div>
    </div>
  );
}

export default SignUp;
