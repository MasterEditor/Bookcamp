import { ChangeEvent, useState } from "react";
import { IValidationResult, useValidation } from "./useValidation";

export interface IInputResult {
  bind: {
    value: string;
    onChange: (e: ChangeEvent<HTMLInputElement>) => void;
    onBlur: (e: ChangeEvent<HTMLInputElement>) => void;
  };
  isDirty: boolean;
  valid: IValidationResult;
}

export const useInput = (
  initialValue: string,
  regexName: string
): IInputResult => {
  const [value, setValue] = useState(initialValue);
  const [isDirty, setDirty] = useState(false);

  const valid = useValidation(value, regexName);

  const onChange = (e: ChangeEvent<HTMLInputElement>) => {
    setValue(e.target.value);
  };

  const onBlur = (e: ChangeEvent<HTMLInputElement>) => {
    setDirty(true);
  };

  return {
    bind: { value, onChange, onBlur },
    isDirty,
    valid,
  };
};
