import random as rd
secret_number = rd.randint(1, 100)
attempts_left = 3
def random_number_game():
    print("Hello! I will pick a number, try to guess it!")
    print("Type 'exit' if you give up >:)")
    while attempts_left > 0:
        guess = (input("Try Guessing!: "))
        if guess > secret_number: 
            guess = (input("Too high! Guess again! "))
        elif guess < secret_number:
            int(guess) = (input("Too low! Guess again!"))
        elif guess == secret_number:
            print(f"You guessed correctly! Congratuations for guessing", secret_number)
        if guess == "exit":
            break
            print(f"oh well, the odds were not in your favor...")
    
random_number_game()