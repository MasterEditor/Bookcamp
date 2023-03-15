import { useEffect, useState } from "react";
import {
  emailRegex,
  maxDate,
  minDate,
  passwordRegex,
  textRegex,
  userNameRegex,
} from "../constants/regularExpression";

export const EMAIL_REGEX = "email";
export const NAME_REGEX = "name";
export const PASSWORD_REGEX = "password";
export const BIRTHDAY = "birthday";
export const REQUIRED = "required";
export const TEXT_REGEX = "text";

export interface IValidationResult {
  isRegexPass: boolean;
  errorMessage: string;
}

export const useValidation = (
  value: string,
  regexName: string
): IValidationResult => {
  const [validationResult, setValidationResult] = useState<IValidationResult>({
    isRegexPass: true,
    errorMessage: "",
  });

  let errorMessage = "";
  let result: RegExpMatchArray | null = null;

  useEffect(() => {
    switch (regexName) {
      case EMAIL_REGEX:
        result = value.match(emailRegex);
        errorMessage = "Email is not valid";
        setValidationResult({
          isRegexPass: result !== null,
          errorMessage,
        });
        break;
      case TEXT_REGEX:
        result = value.match(textRegex);
        errorMessage = "Must be no shorter than 2 and no longer than 32 characters. Only alphabet, digits and symbol of '-'";
        setValidationResult({
          isRegexPass: result !== null,
          errorMessage,
        });
        break;
      case NAME_REGEX:
        result = value.match(userNameRegex);
        errorMessage =
          "Name must be no shorter than 2 and no longer than 32 characters. Only eng alphabet, digits and symbol of '-'";
        setValidationResult({
          isRegexPass: result !== null,
          errorMessage,
        });
        break;
      case PASSWORD_REGEX:
        result = value.match(passwordRegex);
        errorMessage =
          "Password must be no shorter than eight characters, at least one uppercase letter, one lowercase letter and one number";
        setValidationResult({
          isRegexPass: result !== null,
          errorMessage,
        });
        break;
      case BIRTHDAY:
        errorMessage = "Please enter correct date to this field";
        setValidationResult({
          isRegexPass:
            value.length > 0 &&
            new Date(value) > new Date(minDate) &&
            new Date(value) < new Date(maxDate),
          errorMessage,
        });
        break;
      case REQUIRED:
        errorMessage = "Please enter right value";
        setValidationResult({
          isRegexPass: value.length > 0,
          errorMessage,
        });
    }
  }, [value]);

  return validationResult;
};
