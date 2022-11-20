import { IListItem } from "./IListItem";

export const listItems: IListItem[] = [
  {
    id: 1,
    name: "Fantasy",
    imgStyle:
      "bg-[url('pics/fantasy.png')] bg-contain bg-no-repeat bg-center transition-all ease-in-out duration-700 w-[40rem] h-[30rem] drop-shadow-[0_35px_35px_rgba(174,85,183,0.4)]",
    selected: true,
  },
  {
    id: 2,
    name: "History",
    imgStyle:
      "bg-[url('pics/history.png')] bg-contain bg-no-repeat bg-center transition-all ease-in-out duration-700 w-[30rem] h-[25rem] drop-shadow-[0_35px_35px_rgba(199,139,70,0.4)]",
    selected: false,
  },
  {
    id: 3,
    name: "Detective",
    imgStyle:
      "bg-[url('pics/detective.png')] bg-contain bg-no-repeat bg-center transition-all ease-in-out duration-700 w-[25rem] h-[35rem] drop-shadow-[0_35px_35px_rgba(29,53,87,0.4)]",
    selected: false,
  },
  {
    id: 4,
    name: "Romantic",
    imgStyle:
      "bg-[url('pics/romantic.png')] bg-contain bg-no-repeat bg-center transition-all ease-in-out duration-700 w-[25rem] h-[35rem] drop-shadow-[0_35px_35px_rgba(214,40,40,0.4)]",
    selected: false,
  },
];
