import { ReviewType } from "./ReviewType";

export interface IReview {
    id?: string;
    title: string;
    body: string;
    type: ReviewType;
    addedAt?: string;
    likes?: string[];
    dislikes?: string[];
    bookId?: string;
    userName?: string;
    imageUrl?: string;
    addedTime?: string;
  }