export interface IReview {
    id?: string;
    title: string;
    body: string;
    type: number;
    likes?: string[];
    dislikes?: string[];
    bookId?: string;
    userName?: string;
    imageUrl?: string;
    addedTime?: string;
  }