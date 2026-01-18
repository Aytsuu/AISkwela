import z from "zod";

export const enrollmentSchema = z.object({
  classId: z.string().min(1, "Class ID is required")
})