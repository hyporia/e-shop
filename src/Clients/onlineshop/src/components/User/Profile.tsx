import { useEffect, useState } from "react";
import { getUser, User } from "../../services/AuthService";

const Profile = (): JSX.Element => {
    const [user, setUser] = useState<User | null>(null);
    useEffect(() => {
        const checkUser = async () => {
            const user = await getUser();
            setUser(user);
        };

        checkUser();
    }, []);

    return (
        <div>
            {user ? (
                <div>
                    <h1>Welcome {user.name}</h1>
                    <p>Your user id is: {user.id}</p>
                </div>
            ) : (
                <h1>Not logged in</h1>
            )}
        </div>
    );
};

export default Profile;
