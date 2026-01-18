"use client"

import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from "../../../components/ui/form";
import { z } from 'zod';
import { zodResolver } from "@hookform/resolvers/zod"
import { Input } from "../../../components/ui/input";
import { loginSchema } from "../../../schemas/auth.schema";
import { useForm } from "react-hook-form";
import { Button } from "../../../components/ui/button";
import { useLayoutEffect, useState } from "react";
import { useLogin } from "../../../hooks/useAuth";
import { useRouter } from "next/navigation";

export default function LoginPage() {
  const router = useRouter();
  const form = useForm<z.infer<typeof loginSchema>>({
    resolver: zodResolver(loginSchema),
    defaultValues: {
      username: "",
      password: "",
    },
  })

  const [isSubmitting, setIsSubmitting] = useState(false);
  const { mutateAsync: login } = useLogin();

  // Handlers
  const handleLogin = async () => {
    if (!(await form.trigger())){
      return
    }

    try {
      setIsSubmitting(true);
      await login(form.getValues());
    } catch (err) {
      alert("Failed to login. Please try again.");
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <div>
      <h1>Login Page</h1>
      <Form {...form}>
        <form onSubmit={(e) => {
          e.preventDefault();
          handleLogin();
        }}>
          <FormField
            control={form.control}
            name="username"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Username</FormLabel>
                <FormControl>
                  <Input placeholder="Enter your username" {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name="password"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Password</FormLabel>
                <FormControl>
                  <Input type="password" placeholder="Enter your password" {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <Button type={"submit"}>
            Login
          </Button>
        </form>
      </Form>
    </div>
  )
}