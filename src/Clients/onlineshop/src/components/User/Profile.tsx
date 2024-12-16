import { useEffect, useState } from "react";
import { getUser } from "../../services/AuthService";
import { User, UserProfile } from "oidc-client-ts";
import { json } from "stream/consumers";

const Profile = (): JSX.Element => {
	const [user, setUser] = useState<UserProfile | null>(null);
	useEffect(() => {
		const checkUser = async () => {
			const user = await getUser();
			setUser(user?.profile ?? null);
		};

		checkUser();
	}, [user]);

	return (
		<div>
			<h1>Profile</h1>
			<p>{user}</p>
		</div>
	);
};

export default Profile;
