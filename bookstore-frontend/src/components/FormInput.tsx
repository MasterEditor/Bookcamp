import React, { memo } from "react";
import { IInputResult } from "../hooks/useInput";

interface IFormInput {
  id: string;
  type: string;
  placeholder?: string;
  name?: string;
  input: IInputResult;
  max?: string;
  min?: string;
}

function FormInput(props: IFormInput) {
  return (
    <div className="mb-4">
      <label
        className="block text-gray-700 text-sm font-bold mb-2"
        htmlFor="username"
      >
        {props.placeholder}
      </label>
      <input
        className="shadow appearance-none text-[16px] border rounded w-full px-3 py-2 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
        id={props.id}
        type={props.type}
        placeholder={props.placeholder}
        {...props.input.bind}
        max={props.max}
        min={props.min}
      />
      {props.input.isDirty && !props.input.valid.isRegexPass && (
        <p className="text-red-500 mt-1 text-xs italic">
          {props.input.valid.errorMessage}
        </p>
      )}
    </div>
  );
}

export default memo(FormInput);
