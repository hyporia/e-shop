import { useEffect, useState } from "react";
import { getUser } from "../../services/AuthService";
import { User } from "oidc-client-ts";
import { json } from "stream/consumers";

const Profile = (): JSX.Element => {
	const [profile, setProfile] = useState<string | undefined>();
	useEffect(() => {
		const checkUser = async () => {
			const user = await getUser();
			setProfile(JSON.stringify(user?.profile));
		};

		checkUser();
	}, [profile]);

	return (
		<div>
			<h1>Profile</h1>
			<p>{profile}</p>
		</div>
	);
};

export default Profile;
