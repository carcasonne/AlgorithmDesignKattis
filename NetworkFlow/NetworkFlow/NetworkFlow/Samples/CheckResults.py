# Function to check if any two lines in a file are equal
def check_equal_lines(filename):
    line_set = set()
    line_number = 0

    with open(filename, 'r') as file:
        for line in file:
            line_number += 1
            line = line.strip().split(' ')[4]  # Remove leading/trailing whitespace
            if line in line_set:
                return True, line_number  # Found a duplicate line
            line_set.add(line)

    return False, None  # No equal lines found

def print_result(result, line_number, filename):
    if result:
        print(f"Equal results found in the file at line {line_number}.")
    else:
        print(f"No equal results found in the file: {filename}.")

# Specify the file to check

# Call the function to check for equal lines
result, line_number = check_equal_lines("result_100.txt")
print_result(result, line_number, "result_100.txt")
result, line_number = check_equal_lines("result_1000.txt")
print_result(result, line_number, "result_1000.txt")
result, line_number = check_equal_lines("result_2500.txt")
print_result(result, line_number, "result_2500.txt")
result, line_number = check_equal_lines("result_5000.txt")
print_result(result, line_number, "result_5000.txt")

