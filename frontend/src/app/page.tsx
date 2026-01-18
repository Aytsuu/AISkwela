
import Link from "next/link";

export default function Home() {
  return (
      <div>
          <h1>Welcome!</h1>
          <Link href="authentication/login">Login</Link>
          <Link href="authentication/signup">Signup</Link>
      </div>
  );
}
