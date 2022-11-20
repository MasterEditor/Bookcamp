export interface IBook {
  id: string;
  name: string;
  author: string;
  genre: string;
  price: number;
  rating?: number;
  about?: string;
  language?: string;
  pages?: number;
  publishDate?: string;
  cover?: string;
  fragmentPaths?: string[];
}
