import random

# Function to generate n lines of pairs of random numbers and save them to a text file
def generate_number_pairs(n, filename):
    with open(filename, 'w') as file:
        file.write(f"{str(n)}\n")
        for _ in range(n):
            number1 = random.randint(-10000, 10000)
            number2 = random.randint(-10000, 10000)
            file.write(f"{number1} {number2}\n")

# Define the number of lines and the output file name
n = 2500  # Change this to the desired number of lines
output_file = f"sample_{str(n)}.txt"

# Call the function to generate and save the pairs of numbers
generate_number_pairs(n, output_file)

print(f"{n} lines of number pairs have been written to {output_file}.")