import React, { ReactElement, useEffect, useState } from "react";
import { FaSearch } from "react-icons/fa";
import { booksApi } from "../services/booksApi";
import "./modal.css";
import ModalReviews from "./ModalReviews";

interface ModalProps {
  visible: boolean;
  title: string;
  content: ReactElement | string;
  onClose: () => void;
}

function Modal({
  visible = false,
  title = "",
  content = "",
  onClose,
}: ModalProps) {
  const onKeydown = ({ key }: KeyboardEvent) => {
    switch (key) {
      case "Escape":
        onClose();
        break;
    }
  };

  useEffect(() => {
    document.addEventListener("keydown", onKeydown);
    if (visible) {
      document.body.style.overflow = "hidden";
    }
    return () => {
      document.body.style.overflow = "unset";
      document.removeEventListener("keydown", onKeydown);
    };
  });

  if (!visible) return null;

  return (
    <div className="modal" onClick={onClose}>
      <div className="modal-dialog" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h3 className="modal-title">{title}</h3>
          <span className="modal-close" onClick={onClose}>
            &times;
          </span>
        </div>
        <div className="modal-body">
          <div className="modal-content">{content}</div>
        </div>
      </div>
    </div>
  );
}

export default Modal;
