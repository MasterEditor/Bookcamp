export const userNameRegex = /^[a-zA-Z0-9-]{2,32}$/;
export const textRegex =
  /^[a-zA-ZА-Яа-я0-9- ]{2,32}$/;
export const emailRegex =
  /^.{1,64}[@]{1}[a-zA-Z0-9-]{2,}([.]{1}[a-zA-Z0-9-]+)+$/;
export const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,32}$/;
export const maxDate = "2022-01-01";
export const minDate = "1940-01-01";
