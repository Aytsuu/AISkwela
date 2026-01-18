import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { ClassroomService } from "../services/classroom.service";
import { ClassroomResponse } from "../types/classroom";
import { useAuth } from "../components/context/AuthContext";

export const useCreateClassroom = () => {
  const queryClient = useQueryClient();
  const { user } = useAuth();
  return useMutation({
    mutationFn: ClassroomService.create,
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ['classroomsByUserId'] });
      queryClient.setQueryData(['classroomById', user.userId], (old: ClassroomResponse[] = []) => [
        ...old,
        data
      ])
    }
  })
}

export const useGetClassroomsByUserId = (userId: string, role: string) => {
  return useQuery({
    queryKey: ['classroomsByUserId', userId],
    queryFn: () => ClassroomService.getByUserId(userId),
    staleTime: 5000,
    enabled: !!userId && role.toLowerCase() === 'teacher',
    retry: false
  })
}

export const useGetClassroomData = (classId: string, userId: string, role: string) => {
  return useQuery({
    queryKey: ['classroomData', classId, userId],
    queryFn: () => ClassroomService.getData(classId, userId, role),
    staleTime: 5000,
    enabled: !!classId && !!userId
  })
}