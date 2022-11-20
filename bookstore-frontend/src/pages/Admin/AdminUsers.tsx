import React, { useEffect, useState } from "react";
import AdminHeader from "../../components/AdminHeader";
import UserCard from "../../components/UserCard";
import { authApi } from "../../services/authApi";
import { IUser } from "../../services/userSlice";

function AdminUsers() {
  const [users, setUsers] = useState<IUser[]>([]);

  const { data, isSuccess } = authApi.useGetAllUsersQuery();

  useEffect(() => {
    if (isSuccess) {
      setUsers(data!);
    }
  }, [data, isSuccess]);

  return (
    <div className="flex flex-col h-full justify-between">
      <AdminHeader />
      <div className="w-full flex flex-wrap justify-center mt-10">
        {users && users.length > 0 ? (
          users.map((item, key) => (
            <div className="basis-1/6 m-5 but-anim" key={key}>
              <UserCard {...item} />
            </div>
          ))
        ) : (
          <p className="font-bold text-3xl">No users</p>
        )}
      </div>
    </div>
  );
}

export default AdminUsers;
