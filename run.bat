@echo off

start "AlgoLab.API" cmd /k "dotnet run --project src\Presentation\AlgoLab.API"

start "algo-lab-frontend" cmd /k "cd algo-lab-frontend && npm run dev"

exit