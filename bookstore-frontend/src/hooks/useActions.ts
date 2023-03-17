import { bindActionCreators } from "@reduxjs/toolkit";
import { useDispatch } from "react-redux";
import { reviewsActions } from "../services/reviewsSlice";
import { userActions } from "../services/userSlice";

const actions = {
  ...userActions,
  ...reviewsActions
};

export const useActions = () => {
  const dispatch = useDispatch();

  return bindActionCreators(actions, dispatch);
};
