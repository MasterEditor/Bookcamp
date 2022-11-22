import { createSlice, PayloadAction } from "@reduxjs/toolkit";

interface IUserState {
  user: IUser;
}

export interface IUser {
  email?: string;
  name?: string;
  birth?: string;
  imageUrl?: string;
  gender?: number;
  registerDate?: string;
  favourites?: string[];
}

const initialState: IUserState = {
  user: {},
};

export const userSlice = createSlice({
  name: "user",
  initialState,
  reducers: {
    updateUser(state, action: PayloadAction<IUser>) {
      state.user = action.payload;
      localStorage.setItem("user_name", action.payload.name!);
    },
    storeImage(state, action: PayloadAction<string>) {
      state.user.imageUrl = action.payload;
    },
    updateName(state, action: PayloadAction<string>) {
      state.user.name = action.payload;
    },
    logoutUser(state) {
      localStorage.clear();
      state = initialState;
    },
  },
});

export const userActions = userSlice.actions;
export const userReducer = userSlice.reducer;
