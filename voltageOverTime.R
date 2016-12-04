setwd("~/Desktop/Academics/ComputerScience")
dt = read.csv("vot.csv", header = TRUE)
x = dt$Time
y = dt$Voltage
plot(x, y)
m4 = nls(y ~ a*(x^3) + b*(x^2) + c*x + d, start = list(a = 0.01, b = 0.01, c = 0.01, d = 16))
summary(m4)
cor(y, predict(m4))
m5 = nls(y ~ a*(x^3) + b*(x^2) + c*x + d + e*(x^4) + f*(x^5), start = list(a = 0.01, b = 0.01, c = 0.01, d = 16, e = 0.01, f = 0.01))
summary(m5)
cor(y, predict(m5))
z = predict(m5)
# You can take x and z to draw the plots.
# The model we're using is m5. y = -1.141e-0 * x^5 + 1.712e-01 * x^4 + -9.920e-01 * x^3 + 2.764e+00 * x^2 + -3.750e+00 * x + 1.406e+01
# Since we don't have time to get data of different temperature, power and other factors, the whole idea is this:
# We will create different models for different life cycle numbers and users can enter corresponding life cycle numbers to get the predicted voltage at chose time point.