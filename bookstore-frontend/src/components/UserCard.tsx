import { useEffect } from "react";
import { FaUserCircle } from "react-icons/fa";
import { authApi } from "../services/authApi";
import { IUser } from "../services/userSlice";

function UserCard(props: IUser) {
  const [deleteUser, { isSuccess }] = authApi.useDeleteUserMutation();
  const handleDeleteUser = () => {
    const res = window.confirm("Are you sure?");

    if (res) {
      deleteUser(props.email!);
    }
  };

  useEffect(() => {
    if (isSuccess) {
      window.location.reload();
    }
  }, [isSuccess]);

  return (
    <div className="flex flex-col rounded-lg bg-white w-full h-full p-5 shadow-lg">
      <div className="flex flex-row justify-end">
        <p
          className="underline text-red-800 cursor-pointer"
          onClick={handleDeleteUser}
        >
          Delete
        </p>
      </div>
      <div className="flex flex-col items-center mt-8 justify-center">
        {props.imageUrl ? (
          <img src={props.imageUrl} className="rounded-full h-20 w-20" />
        ) : (
          <FaUserCircle className="text-[5rem]" />
        )}
        <p className="text-center mt-3 font-bold text-lg w-52 whitespace-nowrap text-ellipsis overflow-hidden">
          {props.name}
        </p>
        <p className="text-center w-52 whitespace-nowrap text-ellipsis overflow-hidden">
          {props.email}
        </p>
      </div>
    </div>
  );
}

export default UserCard;
