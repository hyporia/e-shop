import { useEffect, useState } from "react";
import { getUser } from "../../services/AuthService";
import { User } from "oidc-client-ts";

const Profile = (): JSX.Element => {
	const [email, setEmail] = useState<string | undefined>();
	useEffect(() => {
		const checkUser = async () => {
			const user = await getUser();
			setEmail(user?.profile?.email);
		};

		checkUser();
	}, [email]);

	return (
		<div>
			<h1>Profile</h1>
			<p>Email: {email}</p>
		</div>
	);
};

export default Profile;
