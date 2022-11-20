import React from "react";
import { IBook } from "../models/IBook";

function SingleBook(props: IBook) {
  return (
    <div className="flex flex-col items-center w-full h-full p-3">
      <img src={props.cover} className="w-full h-[23rem]" />
      <p className="mt-2 mb-1 italic text-sm text-[#e29578]">{props.genre}</p>
      <p className="font-bold mb-1 w-[15rem] text-center">{props.author}</p>
      <p className="font-semibold mb-1 w-[15rem] text-center whitespace-nowrap text-ellipsis overflow-hidden text-[#264653]">
        {props.name}
      </p>
      <p className="font-light text-lg w-[15rem] text-center">
        {props?.price !== 0 ? `${props?.price}$` : "free"}
      </p>
    </div>
  );
}

export default SingleBook;
