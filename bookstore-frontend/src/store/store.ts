import { combineReducers, configureStore } from "@reduxjs/toolkit";
import { authApi } from "../services/authApi";
import { booksApi } from "../services/booksApi";
import { userSlice } from "../services/userSlice";

export const rootReducer = combineReducers({
  [authApi.reducerPath]: authApi.reducer,
  [booksApi.reducerPath]: booksApi.reducer,
  [userSlice.name]: userSlice.reducer,
});

export const setUpStore = () => {
  return configureStore({
    reducer: rootReducer,
    middleware: (getDefaultMiddleware) =>
      getDefaultMiddleware()
        .concat(authApi.middleware)
        .concat(booksApi.middleware),
  });
};

export type RootState = ReturnType<typeof rootReducer>;
// export type AppStore = ReturnType<typeof setUpStore>;
// export type AppDispatch = AppStore["dispatch"];
