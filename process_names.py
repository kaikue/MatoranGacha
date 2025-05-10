import json

syllable_frequencies = {}

with open("name_syllables.txt", "r") as file:
    for line in file:
        syllables = line.strip().split(" ")
        for syllable in syllables:
            if syllable == "":
                continue
            if syllable not in syllable_frequencies:
                syllable_frequencies[syllable] = 0
            syllable_frequencies[syllable] += 1
#data_list = []
#for (syllable, frequency) in syllable_frequencies.items():
#    data_list.append({"syllable": syllable, "frequency": frequency})
#json_data = json.dumps({"items": data_list})
#with open("syllable_frequencies.json", "w") as output_file:
#    output_file.write(json_data)

for (syllable, frequency) in syllable_frequencies.items():
    print("{ \"" + syllable + "\", " + str(frequency) + " },")
