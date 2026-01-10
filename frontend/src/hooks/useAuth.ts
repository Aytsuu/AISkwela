import { useMutation } from "@tanstack/react-query"
import { AuthService } from "../services/auth.service"
import Cookies from 'js-cookie';
import { useRouter } from "next/navigation";

export const useLogin = () => {
  const router = useRouter();
  return useMutation({
    mutationFn: AuthService.login,
    onSuccess: (data) => {
      const sixtyMinutes = new Date(new Date().getTime() + 10 * 60 * 1000);
      Cookies.set('accessToken', data.token, {
        expires: sixtyMinutes, // 10 mins
        secure: true,
        sameSite: 'Strict'
      })

       router.push('/dashboard')

    }
  })
}

export const useSignup = () => {
  return useMutation({
    mutationFn: AuthService.signup,
  })
}