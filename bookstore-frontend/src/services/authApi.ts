import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { Blob } from "buffer";
import { ILogin } from "../models/ILogin";
import { IResponse } from "../models/IResponse";
import { ISignup } from "../models/ISignup";
import { IUser } from "./userSlice";

export const BASE_URL = process.env.REACT_APP_SERVER_ENDPOINT as string;

const baseQuery = fetchBaseQuery({
  baseUrl: `${BASE_URL}/api/user/`,
  credentials: "include",
});

// const baseQueryWithReAuth: BaseQueryFn<
//   string | FetchArgs,
//   unknown,
//   FetchBaseQueryError
// > = async (args, api, extraOptions) => {
//   let result = await baseQuery(args, api, extraOptions);

//   if (result?.error?.status === 403) {
//     const refreshResult = await baseQuery("refresh", api, extraOptions);

//     if (refreshResult?.data) {
//       api.dispatch(userActions.updateToken(refreshResult.data as string));

//       result = await baseQuery(args, api, extraOptions);
//     } else {
//       api.dispatch(userActions.logout());
//     }
//   }

//   return result;
// };

export const authApi = createApi({
  reducerPath: "authApi",
  baseQuery: baseQuery,
  endpoints: (build) => ({
    signupUser: build.mutation<IResponse<string>, ISignup>({
      query: (body) => ({
        url: `signup`,
        method: "POST",
        body,
      }),
    }),
    loginUser: build.mutation<IResponse<string>, ILogin>({
      query: (body) => ({
        url: `login`,
        method: "POST",
        body,
      }),
    }),
    me: build.query<IUser, void>({
      query: () => ({
        url: `me`,
      }),
      transformResponse: (response: IResponse<IUser>) => response.body,
    }),
    updateImage: build.mutation<IResponse<string>, FormData>({
      query: (body) => ({
        url: `image`,
        method: "POST",
        body,
      }),
    }),
    updateUserName: build.mutation<void, object>({
      query: (body) => ({
        url: `change-name`,
        method: "PUT",
        body,
      }),
    }),
    addToFavourite: build.mutation<void, { bookId: string }>({
      query: (body) => ({
        url: `add-favourite`,
        method: "POST",
        body,
      }),
    }),
    removeFromFavourite: build.mutation<void, { bookId: string }>({
      query: (body) => ({
        url: `remove-favourite`,
        method: "POST",
        body,
      }),
    }),
    getAllUsers: build.query<IUser[], void>({
      query: () => ({
        url: `users`,
        method: "GET",
      }),
      transformResponse: (response: IResponse<IUser[]>) => response.body,
    }),
    deleteUser: build.mutation<void, string>({
      query: (email) => ({
        url: `${email}`,
        method: "DELETE",
      }),
    }),
  }),
});
